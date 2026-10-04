package com.example.employeedigitalhandbook.api

import com.google.gson.annotations.SerializedName

//data class ApiLoginResponse(
//    val success: Boolean,
//    val message: String,
//    val user: ApiUser?
//)

data class ApiLoginResponse(
    @SerializedName("userId")
    val userId: Int? = null,

    @SerializedName("email")
    val email: String? = null,

    @SerializedName("name")
    val name: String? = null,

//    @SerializedName("fullName")
//    val fullName: String? = null,

    @SerializedName("token")
    val token: String? = null,

//    @SerializedName("user")
//    val user: ApiUser? = null
)