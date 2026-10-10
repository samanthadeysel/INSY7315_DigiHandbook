package com.example.employeedigitalhandbook.sessions

import android.content.Context
import android.content.SharedPreferences
import android.util.Log
import com.example.employeedigitalhandbook.api.ApiClient
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.text.SimpleDateFormat
import java.util.Date
import java.util.Locale
import java.util.TimeZone

class SessionManager(context: Context) {

    private val prefs: SharedPreferences = context.getSharedPreferences(
        "user_session_prefs",
        Context.MODE_PRIVATE
    )

    companion object {
        private const val KEY_USER_ID = "key_user_id"
        private const val KEY_EMAIL = "key_email"
        private const val KEY_FULL_NAME = "key_full_name"
        private const val KEY_IS_LOGGED_IN = "key_is_logged_in"

        // Tracking state
        private var sessionStartTime: Date? = null
        private val visits = mutableListOf<FragmentVisit>()
        private var activeFragmentName: String? = null
        private var activeFragmentEntryTime: Date? = null

        private val isoFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.US).apply {
            timeZone = TimeZone.getTimeZone("UTC")
        }
    }

    fun saveSession(session: UserSession) {
        prefs.edit().apply {
            putInt(KEY_USER_ID, session.userId.toIntOrNull() ?: 1)
            putString(KEY_EMAIL, session.email)
            putString(KEY_FULL_NAME, session.fullName)
            putBoolean(KEY_IS_LOGGED_IN, true)
            apply()
        }
        startTracking()
    }

    fun getSession(): UserSession? {
        if (!isLoggedIn()) return null

        val userIdInt = prefs.getInt(KEY_USER_ID, -1)
        if (userIdInt == -1) return null

        val email = prefs.getString(KEY_EMAIL, "") ?: ""
        val fullName = prefs.getString(KEY_FULL_NAME, "") ?: ""

        return UserSession(
            userId = userIdInt.toString(),
            email = email,
            fullName = fullName
        )
    }

    fun isLoggedIn(): Boolean {
        return prefs.getBoolean(KEY_IS_LOGGED_IN, false)
    }

    fun startTracking() {
        sessionStartTime = Date()
        visits.clear()
    }

    fun onFragmentResumed(fragmentName: String) {
        if (!isLoggedIn()) return
        onFragmentPaused() // Complete active screen visit

        activeFragmentName = fragmentName
        activeFragmentEntryTime = Date()
    }

    fun onFragmentPaused() {
        val name = activeFragmentName ?: return
        val entryTime = activeFragmentEntryTime ?: return
        val exitTime = Date()

        val durationSec = (exitTime.time - entryTime.time) / 1000.0

        if (durationSec > 0.5) {
            val visit = FragmentVisit(
                fragmentName = name,
                enteredAt = isoFormat.format(entryTime),
                exitedAt = isoFormat.format(exitTime),
                durationSeconds = durationSec
            )
            visits.add(visit)
        }

        activeFragmentName = null
        activeFragmentEntryTime = null
    }

    fun endAndSyncSession() {
        val session = getSession() ?: return
        val start = sessionStartTime ?: return

        onFragmentPaused()

        val endTime = Date()
        val totalDurationSec = (endTime.time - start.time) / 1000.0

        val payload = UserSessionPayload(
            userId = session.userId.toIntOrNull() ?: 1,
            email = session.email,
            fullName = session.fullName,
            startTime = isoFormat.format(start),
            endTime = isoFormat.format(endTime),
            totalDurationSeconds = totalDurationSec,
            visits = visits.toList()
        )

        CoroutineScope(Dispatchers.IO).launch {
            try {
                ApiClient.apiService.logUserSession(payload)
            } catch (e: Exception) {
                Log.e("SessionManager", "Sync Error: ${e.localizedMessage}")
            }
        }

        visits.clear()
        sessionStartTime = null
    }

    fun clearSession() {
        endAndSyncSession()
        prefs.edit().clear().apply()
    }
}