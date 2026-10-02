package com.example.employeedigitalhandbook.sessions

data class UserSession(
    val userId: String,
    val email: String,
    val employeeId: String,
    val fullName: String
)