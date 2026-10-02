package com.example.employeedigitalhandbook.data

import com.google.gson.annotations.SerializedName

data class Resource(
    @SerializedName("id")
    val id: Int,

    @SerializedName("title")
    val title: String,

    @SerializedName("category")
    val category: String,

    @SerializedName("description")
    val description: String?,

    @SerializedName("resourceUrl")
    val resourceUrl: String?,

    @SerializedName("breadcrumbPath")
    val breadcrumbPath: String?
)