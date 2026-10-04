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
import com.example.employeedigitalhandbook.adapters.QuizAdapter
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.databinding.FragmentQuizListBinding
import kotlinx.coroutines.launch

class QuizListFragment : Fragment() {

    private var _binding: FragmentQuizListBinding? = null
    private val binding get() = _binding!!

    private lateinit var quizAdapter: QuizAdapter

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentQuizListBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        setupRecyclerView()
        setupListeners()
        setupBottomNavigation()
        fetchQuizzes()
    }

    private fun setupRecyclerView() {
        quizAdapter = QuizAdapter(emptyList()) { quiz ->
            val bundle = bundleOf("quizId" to quiz.quizId)
            findNavController().navigate(R.id.action_quizListFragment_to_takeQuizFragment, bundle)
        }

        binding.recyclerViewQuizzes.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = quizAdapter
        }
    }

    private fun setupListeners() {
        binding.backArrowImageView.setOnClickListener {
            findNavController().navigateUp()
        }
    }

    private fun setupBottomNavigation() {
        binding.bottomNavigation.selectedItemId = R.id.nav_home
        binding.bottomNavigation.setOnItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_home -> {
                    findNavController().navigate(R.id.action_quizListFragment_to_homePageFragment)
                    true
                }
                R.id.nav_menu -> {
                    findNavController().navigate(R.id.action_quizListFragment_to_settingsFragment)
                    true
                }
                else -> false
            }
        }
    }

    private fun fetchQuizzes() {
        lifecycleScope.launch {
            try {
                val response = ApiClient.apiService.getQuizzes()
                val apiResponse = response.body()

                if (response.isSuccessful && apiResponse != null && apiResponse.data != null) {
                    quizAdapter.updateQuizzes(apiResponse.data)
                } else {
                    Toast.makeText(requireContext(), apiResponse?.message ?: "Failed to load quizzes", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}