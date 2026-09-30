package com.example.employeedigitalhandbook.repositories

import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.api.ApiDoctor

class DoctorRepository {
    suspend fun fetchDoctors(): DoctorResult {
        return try {
            val response = ApiClient.apiService.getDoctors()
            if (response.isSuccessful && response.body() != null) {
                DoctorResult.Success(response.body()!!)
            } else {
                DoctorResult.Error("Failed to load doctors from database.")
            }
        } catch (e: Exception) {
            DoctorResult.Error("Network error: ${e.localizedMessage}")
        }
    }
}
sealed class DoctorResult {
    data class Success(val doctors: List<ApiDoctor>) : DoctorResult()
    data class Error(val message: String) : DoctorResult()
}