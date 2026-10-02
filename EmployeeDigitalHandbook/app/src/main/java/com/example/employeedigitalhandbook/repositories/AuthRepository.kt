package com.example.employeedigitalhandbook.repositories

import com.example.employeedigitalhandbook.admin.AuthResult
import com.example.employeedigitalhandbook.sessions.UserSession
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.api.ApiLoginRequest

class AuthRepository {
    suspend fun login(email: String, password: String): AuthResult {
        return try {
            val response = ApiClient.apiService.login(ApiLoginRequest(email, password))
            if (response.isSuccessful && response.body()?.success == true) {
                val user = response.body()?.user
                if (user != null) {
                    AuthResult.Success(
                        UserSession(
                            userId = user.userId,
                            email = user.email,
                            employeeId = user.employeeId,
                            fullName = user.fullName
                        )
                    )
                } else {
                    AuthResult.Error("User data is empty.")
                }
            } else {
                AuthResult.Error(response.body()?.message ?: "Invalid email or password.")
            }
        } catch (e: Exception) {
            AuthResult.Error("Could not connect to MVC backend server: ${e.localizedMessage}")
        }
    }
}