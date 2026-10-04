package com.example.employeedigitalhandbook.repositories

import com.example.employeedigitalhandbook.admin.AuthResult
import com.example.employeedigitalhandbook.sessions.UserSession
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.api.ApiLoginRequest

class AuthRepository {
//    suspend fun login(email: String, password: String): AuthResult {
//        return try {
//            val response = ApiClient.apiService.login(ApiLoginRequest(email, password))
//            if (response.isSuccessful && response.body() != null) {
//                val body = response.body()!!
//
//                // Resolve user details whether flat or nested
//                val resolvedEmail = body.email ?: body.user?.email ?: ""
//                val resolvedName = body.name ?: body.fullName ?: body.user?.fullName ?: "Staff Member"
//                val resolvedId = body.userId?.toString() ?: body.user?.userId ?: "1"
//
//                if (resolvedEmail.isNotEmpty()) {
//                    val userSession = UserSession(
//                        userId = resolvedId,
//                        email = resolvedEmail,
//                        fullName = resolvedName,
//                        employeeId = body.user?.employeeId ?: ""
//                    )
//                    AuthResult.Success(userSession)
//                } else {
//                    AuthResult.Error("User data is empty")
//                }
//            } else {
//                AuthResult.Error("Login failed: ${response.message()}")
//            }
//        } catch (e: Exception) {
//            AuthResult.Error("Could not connect to MVC backend server: ${e.localizedMessage}")
//        }
//    }
suspend fun login(email: String, password: String): AuthResult {
    return try {
        val response = ApiClient.apiService.login(ApiLoginRequest(email, password))
        if (response.isSuccessful && response.body() != null) {
            val body = response.body()!!

            // Fallback chain for email: API response -> input email
            val resolvedEmail = body.email?.takeIf { it.isNotBlank() } ?: email.trim()

            // Derive name if empty from the database: e.g., "jane.doe" -> "Jane Doe"
            val derivedName = email.substringBefore("@")
                .split(".", "_", "-")
                .joinToString(" ") { it.replaceFirstChar { char -> char.uppercase() } }

            val resolvedName = body.name?.takeIf { it.isNotBlank() } ?: derivedName

            val resolvedId = body.userId?.toString() ?: "1"

            if (resolvedEmail.isNotEmpty()) {
                val userSession = UserSession(
                    userId = resolvedId,
                    email = resolvedEmail,
                    fullName = resolvedName,
                    employeeId = "EMP-$resolvedId"
                )
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