package com.example.employeedigitalhandbook.data

import com.google.gson.annotations.SerializedName

data class Policy(
    @SerializedName("policyId")
    val id: Int,

    @SerializedName("title")
    val title: String,

    @SerializedName("contentSummary")
    val summary: String?,

    @SerializedName("specificCategory")
    val specificCategory: String?,

    @SerializedName("fileUrl")
    val pdfUrl: String?,

    @SerializedName("categoryId")
    val categoryId: Int?,

    @SerializedName("category")
    val categoryObj: PolicyCategoryData? = null
) {
    val category: String
        get() = specificCategory ?: categoryObj?.categoryName ?: "General"

    val breadcrumbPath: String
        get() = "../$category/$title"
}

data class PolicyCategoryData(
    @SerializedName("categoryId")
    val categoryId: Int,

    @SerializedName("categoryName")
    val categoryName: String
)