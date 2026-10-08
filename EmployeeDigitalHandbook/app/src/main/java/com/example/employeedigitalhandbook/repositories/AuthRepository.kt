package com.example.employeedigitalhandbook.repositories

import com.example.employeedigitalhandbook.admin.AuthResult
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.api.ApiLoginRequest
import com.example.employeedigitalhandbook.sessions.SessionManager
import com.example.employeedigitalhandbook.sessions.UserSession

class AuthRepository(private val sessionManager: SessionManager) {

    suspend fun login(email: String, password: String): AuthResult {
        return try {
            val response = ApiClient.apiService.login(ApiLoginRequest(email, password))
            if (response.isSuccessful && response.body() != null) {
                val body = response.body()!!

                val resolvedEmail = body.email?.takeIf { it.isNotBlank() } ?: email.trim()

                val derivedName = email.substringBefore("@")
                    .split(".", "_", "-")
                    .joinToString(" ") { part ->
                        part.replaceFirstChar { if (it.isLowerCase()) it.titlecase() else it.toString() }
                    }

                val resolvedName = body.name?.takeIf { it.isNotBlank() } ?: derivedName

                val resolvedId = body.userId?.toString() ?: "1"

                if (resolvedEmail.isNotEmpty()) {
                    val userSession = UserSession(
                        userId = resolvedId,
                        email = resolvedEmail,
                        fullName = resolvedName
                    )

                    sessionManager.saveSession(userSession)

                    AuthResult.Success(userSession)
                } else {
                    AuthResult.Error("User data is empty")
                }
            } else {
                AuthResult.Error("Login failed: ${response.message()}")
            }
        } catch (e: Exception) {
            AuthResult.Error("Could not connect to backend server: ${e.localizedMessage}")
        }
    }
}