package com.example.employeedigitalhandbook.data

import com.google.gson.annotations.SerializedName

data class QuizOption(
    @SerializedName("optionId")
    val optionId: Int,

    @SerializedName("optionText")
    val optionText: String,

    @SerializedName("isCorrect")
    val isCorrect: Boolean = false
)

data class QuizQuestion(
    @SerializedName("questionId")
    val questionId: Int,

    @SerializedName("questionText")
    val questionText: String,

    @SerializedName("options")
    val options: List<QuizOption>
)

data class Quiz(
    @SerializedName("quizId")
    val quizId: Int,

    @SerializedName("title")
    val title: String,

    @SerializedName("topic")
    val topic: String,

    @SerializedName("estimatedMinutes")
    val estimatedMinutes: Int,

    @SerializedName("points")
    val points: Double,

    @SerializedName("passPercentage")
    val passPercentage: Int = 80,

    @SerializedName("questions")
    val questions: List<QuizQuestion> = emptyList()
)

data class QuizSubmission(
    @SerializedName("quizId")
    val quizId: Int,

    @SerializedName("scoreFraction")
    val scoreFraction: String,

    @SerializedName("percentage")
    val percentage: Int,

    @SerializedName("passed")
    val passed: Boolean,

    @SerializedName("cpdPointsEarned")
    val cpdPointsEarned: Double
)