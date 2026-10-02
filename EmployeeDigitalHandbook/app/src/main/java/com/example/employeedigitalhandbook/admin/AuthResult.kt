package com.example.employeedigitalhandbook.admin

import com.example.employeedigitalhandbook.sessions.UserSession

sealed class AuthResult {
    data class Success(val user: UserSession) : AuthResult()
    data class Error(val message: String) : AuthResult()
}