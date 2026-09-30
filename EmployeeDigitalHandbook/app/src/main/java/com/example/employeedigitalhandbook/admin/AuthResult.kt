package com.example.employeedigitalhandbook.admin

sealed class AuthResult {
    data class Success(val user: UserSession) : AuthResult()
    data class Error(val message: String) : AuthResult()
}