package com.example.employeedigitalhandbook.api

data class ApiLoginResponse(
    val success: Boolean,
    val message: String,
    val user: ApiUser?
)