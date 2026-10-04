package com.example.employeedigitalhandbook.api

import com.example.employeedigitalhandbook.data.BragBook
import com.example.employeedigitalhandbook.data.CommunityEvent
import com.example.employeedigitalhandbook.data.Doctor
import com.example.employeedigitalhandbook.data.Policy
import com.example.employeedigitalhandbook.data.Quiz
import com.example.employeedigitalhandbook.data.QuizSubmission
import com.example.employeedigitalhandbook.data.Resource
import com.example.employeedigitalhandbook.api.ApiResponse
import retrofit2.Response
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.Path

interface ApiService {

    //corrected routes for login, community and quizzes

//    @POST("api/authapi/login")
//    suspend fun login(@Body request: ApiLoginRequest): Response<ApiLoginResponse>


    @POST("api/Auth/login")
    suspend fun login(@Body request: ApiLoginRequest): Response<ApiLoginResponse>

    // --- COMMUNITY EVENTS ---
//    @GET("api/CommunityEvents")
//    suspend fun getCommunityEvents(): Response<List<CommunityEvent>>
    @GET("api/Community")
    suspend fun getCommunityEvents(): Response<ApiResponse<List<CommunityEvent>>>

    // --- DOCTORS ---
    @GET("api/Doctors")
    suspend fun getDoctors(): Response<List<Doctor>>

    @GET("api/Doctors/{id}")
    suspend fun getDoctorById(@Path("id") id: Int): Response<Doctor>

    // --- BRAG BOOK ---
    @GET("api/bragbook")
    suspend fun getBragPosts(): Response<List<BragBook>>

    @POST("api/bragbook")
    suspend fun createBragPost(@Body post: BragBook): Response<BragBook>

    // --- QUIZZES ---
//    @GET("api/Quizs")
//    suspend fun getQuizzes(): Response<List<Quiz>>

    @GET("api/Quizzes")
    suspend fun getQuizzes(): Response<List<Quiz>>

    @GET("api/Quizs/{id}")
    suspend fun getQuizById(@Path("id") id: Int): Response<Quiz>

    @POST("api/quizzes/submit")
    suspend fun submitQuizResult(@Body submission: QuizSubmission): Response<QuizSubmission>

    // --- RESOURCE ---
    @GET("api/resources")
    suspend fun getResources(): Response<ApiResponse<List<Resource>>>

    @GET("api/resources/{id}")
    suspend fun getResourceById(@Path("id") id: Int): Response<ApiResponse<Resource>>

    // --- POLICY ---
    @GET("api/policies")
    suspend fun getPolicies(): Response<List<Policy>>

    @GET("api/policies/{id}")
    suspend fun getPolicyById(@Path("id") id: Int): Response<Policy>
}