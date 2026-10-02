package com.example.employeedigitalhandbook.sessions

import android.content.Context
import android.content.SharedPreferences
import com.example.employeedigitalhandbook.sessions.UserSession

class SessionManager(context: Context) {

    private val prefs: SharedPreferences = context.getSharedPreferences(
        "user_session_prefs",
        Context.MODE_PRIVATE
    )

    companion object {
        private const val KEY_USER_ID = "key_user_id"
        private const val KEY_EMAIL = "key_email"
        private const val KEY_EMPLOYEE_ID = "key_employee_id"
        private const val KEY_FULL_NAME = "key_full_name"
        private const val KEY_IS_LOGGED_IN = "key_is_logged_in"
    }

    fun saveSession(session: UserSession) {
        prefs.edit().apply {
            putString(KEY_USER_ID, session.userId)
            putString(KEY_EMAIL, session.email)
            putString(KEY_EMPLOYEE_ID, session.employeeId)
            putString(KEY_FULL_NAME, session.fullName)
            putBoolean(KEY_IS_LOGGED_IN, true)
            apply()
        }
    }

    fun getSession(): UserSession? {
        if (!isLoggedIn()) return null

        val userId = prefs.getString(KEY_USER_ID, null) ?: return null
        val email = prefs.getString(KEY_EMAIL, "") ?: ""
        val employeeId = prefs.getString(KEY_EMPLOYEE_ID, "") ?: ""
        val fullName = prefs.getString(KEY_FULL_NAME, "") ?: ""

        return UserSession(
            userId = userId,
            email = email,
            employeeId = employeeId,
            fullName = fullName
        )
    }

    fun isLoggedIn(): Boolean {
        return prefs.getBoolean(KEY_IS_LOGGED_IN, false)
    }

    fun clearSession() {
        prefs.edit().clear().apply()
    }
}