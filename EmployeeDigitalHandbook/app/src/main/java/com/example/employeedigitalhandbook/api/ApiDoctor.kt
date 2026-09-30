package com.example.employeedigitalhandbook.api

import com.google.gson.annotations.SerializedName

data class ApiDoctor(
    @SerializedName("doctorId")
    val doctorId: Int,

    @SerializedName("doctorImg")
    val doctorImg: String? = null,

    @SerializedName("fName")
    val fName: String,

    @SerializedName("lName")
    val lName: String,

    @SerializedName("email")
    val email: String,

    @SerializedName("phone")
    val phone: String,

    @SerializedName("suiteNumber")
    val suiteNumber: Int
) {
    val fullNameWithTitle: String
        get() = "Dr. $fName $lName"
}