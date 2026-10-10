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
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.data.QuizSubmission
import com.example.employeedigitalhandbook.databinding.FragmentQuizResultBinding
import com.example.employeedigitalhandbook.sessions.SessionManager
import kotlinx.coroutines.launch

class QuizResultFragment : Fragment() {

    private var _binding: FragmentQuizResultBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentQuizResultBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val quizId = arguments?.getInt("quizId") ?: 0
        val quizTitle = arguments?.getString("quizTitle").orEmpty()
        val scoreFraction = arguments?.getString("scoreFraction").orEmpty()
        val scorePercentage = arguments?.getString("scorePercentage") ?: "0%"
        val cpdPoints = arguments?.getString("cpdPoints") ?: "+0.0"
        val passThreshold = arguments?.getString("passThreshold") ?: "80%"
        val passed = arguments?.getBoolean("passed") ?: false

        binding.txtQuizTitle.text = quizTitle
        binding.txtScorePercentage.text = scorePercentage
        binding.txtScoreFraction.text = scoreFraction
        binding.txtCpdPointsEarned.text = cpdPoints
        binding.txtPassThreshold.text = passThreshold

        if (passed) {
            binding.txtResultHeading.text = "Assessment Passed!"
            binding.txtFeedbackMessage.text = "Congratulations! Your CPD points have been recorded to your staff profile."
            binding.btnRetakeQuiz.visibility = View.GONE
        } else {
            binding.txtResultHeading.text = "Assessment Failed"
            binding.txtFeedbackMessage.text = "You did not achieve the minimum required score to earn CPD points. Please review and try again."
            binding.btnRetakeQuiz.visibility = View.VISIBLE
        }

        binding.btnDone.setOnClickListener {
            findNavController().navigate(R.id.action_quizResultFragment_to_quizListFragment)
        }

        binding.btnRetakeQuiz.setOnClickListener {
            val bundle = bundleOf("quizId" to quizId)
            findNavController().navigate(R.id.action_quizResultFragment_to_takeQuizFragment, bundle)
        }

        postQuizResults(quizId, scoreFraction, scorePercentage, passed, cpdPoints)
    }

    private fun postQuizResults(
        quizId: Int,
        fraction: String,
        percentageStr: String,
        passed: Boolean,
        pointsStr: String
    ) {
        val numericPercentage = percentageStr.replace("%", "").toIntOrNull() ?: 0
        val numericPoints = pointsStr.replace("+", "").toDoubleOrNull() ?: 0.0

        val sessionManager = SessionManager(requireContext().applicationContext)
        val userIdInt = sessionManager.getSession()?.userId?.toIntOrNull() ?: 1

        val submission = QuizSubmission(
            quizId = quizId,
            scoreFraction = fraction,
            percentage = numericPercentage,
            passed = passed,
            cpdPointsEarned = numericPoints,
            userId = userIdInt
        )

        lifecycleScope.launch {
            try {
                ApiClient.apiService.submitQuizResult(submission)
            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Result sync error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}