package com.example.employeedigitalhandbook.data

import com.google.gson.annotations.SerializedName

data class Policy(
    @SerializedName("id")
    val id: Int,

    @SerializedName("title")
    val title: String,

    @SerializedName("category")
    val category: String,

    @SerializedName("summary")
    val summary: String?,

    @SerializedName("pdfUrl")
    val pdfUrl: String?,

    @SerializedName("breadcrumbPath")
    val breadcrumbPath: String?
)