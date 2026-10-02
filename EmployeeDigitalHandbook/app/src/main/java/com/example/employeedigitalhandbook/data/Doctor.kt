package com.example.employeedigitalhandbook.data

import com.google.gson.annotations.SerializedName

data class Doctor(
    @SerializedName("doctorId") val doctorId: Int,
    @SerializedName("doctorImg") val doctorImg: String? = "",
    @SerializedName("fName") val fName: String = "",
    @SerializedName("lName") val lName: String = "",
    @SerializedName("email") val email: String = "",
    @SerializedName("phone") val phone: String = "",
    @SerializedName("suiteNumber") val suiteNumber: Int = 0
) {
    val fullNameWithTitle: String
        get() = "Dr. $fName $lName"
}