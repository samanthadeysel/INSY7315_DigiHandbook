package com.example.employeedigitalhandbook.features

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.os.bundleOf
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.adapters.QuizOptionAdapter
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.data.Quiz
import com.example.employeedigitalhandbook.data.QuizOption
import com.example.employeedigitalhandbook.databinding.FragmentQuizTakeBinding
import kotlinx.coroutines.launch

class QuizTakeFragment : Fragment() {

    private var _binding: FragmentQuizTakeBinding? = null
    private val binding get() = _binding!!

    private var currentQuiz: Quiz? = null
    private var currentQuestionIndex = 0
    private var correctAnswersCount = 0
    private var selectedOption: QuizOption? = null

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentQuizTakeBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val quizId = arguments?.getInt("quizId") ?: 0

        binding.btnCloseQuiz.setOnClickListener {
            findNavController().navigateUp()
        }

        binding.btnNextQuestion.setOnClickListener {
            submitCurrentQuestionAnswer()
        }

        loadQuiz(quizId)
    }

    private fun loadQuiz(quizId: Int) {
        lifecycleScope.launch {
            try {
                val response = ApiClient.apiService.getQuizById(quizId)
                val body = response.body()

                if (response.isSuccessful && body?.data != null) {
                    currentQuiz = body.data
                    displayQuestion()
                } else {
                    Toast.makeText(
                        requireContext(),
                        body?.message ?: "Failed to load quiz details (HTTP ${response.code()})",
                        Toast.LENGTH_SHORT
                    ).show()
                }
            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Error loading quiz: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun displayQuestion() {
        val quiz = currentQuiz ?: return
        val questions = quiz.questions

        if (questions.isEmpty()) {
            Toast.makeText(requireContext(), "This quiz has no questions available.", Toast.LENGTH_SHORT).show()
            return
        }

        val question = questions.getOrNull(currentQuestionIndex) ?: return
        selectedOption = null
        binding.btnNextQuestion.isEnabled = false

        binding.txtQuizHeaderTitle.text = quiz.title
        binding.txtQuestionCounter.text = "Question ${currentQuestionIndex + 1} of ${questions.size}"
        binding.txtQuestionText.text = question.questionText

        val adapter = QuizOptionAdapter(question.options) { option ->
            selectedOption = option
            binding.btnNextQuestion.isEnabled = true
        }

        binding.recyclerViewOptions.apply {
            layoutManager = LinearLayoutManager(requireContext())
            this.adapter = adapter
        }

        binding.btnNextQuestion.text = if (currentQuestionIndex == questions.size - 1) {
            "Submit Assessment"
        } else {
            "Next Question"
        }
    }

    private fun submitCurrentQuestionAnswer() {
        val option = selectedOption ?: return
        if (option.isCorrect) {
            correctAnswersCount++
        }

        val quiz = currentQuiz ?: return
        if (currentQuestionIndex < quiz.questions.size - 1) {
            currentQuestionIndex++
            displayQuestion()
        } else {
            navigateToResults()
        }
    }

    private fun navigateToResults() {
        val quiz = currentQuiz ?: return
        val totalQuestions = quiz.questions.size
        val percentage = if (totalQuestions > 0) ((correctAnswersCount.toDouble() / totalQuestions) * 100).toInt() else 0
        val passed = percentage >= quiz.passPercentage

        val bundle = bundleOf(
            "quizId" to quiz.quizId,
            "quizTitle" to quiz.title,
            "scoreFraction" to "You answered $correctAnswersCount out of $totalQuestions questions correctly",
            "scorePercentage" to "$percentage%",
            "cpdPoints" to if (passed) "+${quiz.points}" else "+0.0",
            "passThreshold" to "${quiz.passPercentage}%",
            "passed" to passed
        )

        findNavController().navigate(R.id.action_takeQuizFragment_to_quizResultFragment, bundle)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}