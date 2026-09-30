package com.example.employeedigitalhandbook.admin

data class UserSession(
    val userId: String,
    val email: String,
    val employeeId: String,
    val fullName: String
)