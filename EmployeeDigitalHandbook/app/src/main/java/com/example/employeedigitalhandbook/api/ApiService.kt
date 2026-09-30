package com.example.employeedigitalhandbook.api

import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST

interface ApiService {
    @POST("api/authapi/login")
    suspend fun login(@Body request: ApiLoginRequest): Response<ApiLoginResponse>

    @GET("api/doctorsapi")
    suspend fun getDoctors(): Response<List<ApiDoctor>>
}