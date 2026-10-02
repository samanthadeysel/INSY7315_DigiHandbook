package com.example.employeedigitalhandbook.data

import com.google.gson.annotations.SerializedName

data class CommunityEvent(
    @SerializedName("eventId") val eventId: Int = 0,
    @SerializedName("title") val title: String = "",
    @SerializedName("date") val date: String = "",
    @SerializedName("time") val time: String = "",
    @SerializedName("location") val location: String = "",
    @SerializedName("category") val category: String = ""
)