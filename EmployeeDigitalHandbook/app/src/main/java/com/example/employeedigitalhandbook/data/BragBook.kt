package com.example.employeedigitalhandbook.data

import com.google.gson.annotations.SerializedName

data class BragBook(
    @SerializedName("bragId")
    val bragId: Int = 0,

    @SerializedName("recipientName")
    val recipientName: String,

    @SerializedName("content")
    val content: String,

    @SerializedName("senderType")
    val senderType: String = "Peer",

    @SerializedName("isAnonymous")
    val isAnonymous: Boolean = false,

    @SerializedName("datePosted")
    val datePosted: String? = null
)