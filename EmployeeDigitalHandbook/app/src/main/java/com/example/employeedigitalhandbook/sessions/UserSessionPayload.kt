package com.example.employeedigitalhandbook.sessions

import com.google.gson.annotations.SerializedName

data class UserSessionPayload(
    @SerializedName("userId") val userId: Int,
    @SerializedName("email") val email: String,
    @SerializedName("fullName") val fullName: String,
    @SerializedName("startTime") val startTime: String,
    @SerializedName("endTime") val endTime: String,
    @SerializedName("totalDurationSeconds") val totalDurationSeconds: Double,
    @SerializedName("visits") val visits: List<FragmentVisit>
)

data class FragmentVisit(
    @SerializedName("fragmentName") val fragmentName: String,
    @SerializedName("enteredAt") val enteredAt: String,
    @SerializedName("exitedAt") val exitedAt: String,
    @SerializedName("durationSeconds") val durationSeconds: Double
)

data class UserSession(
    val userId: String,
    val email: String,
    val fullName: String
)