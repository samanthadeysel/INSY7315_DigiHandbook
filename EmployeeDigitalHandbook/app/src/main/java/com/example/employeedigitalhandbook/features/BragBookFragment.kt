package com.example.employeedigitalhandbook.features

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.adapters.BragBookAdapter
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.data.BragBook
import com.example.employeedigitalhandbook.databinding.FragmentBragBookBinding
import kotlinx.coroutines.launch

class BragBookFragment : Fragment() {

    private var _binding: FragmentBragBookBinding? = null
    private val binding get() = _binding!!

    private lateinit var bragAdapter: BragBookAdapter

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentBragBookBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        setupRecyclerView()
        setupListeners()
        //setupBottomNavigation()
        fetchBragPosts()
    }

    private fun setupRecyclerView() {
        bragAdapter = BragBookAdapter(emptyList())
        binding.recyclerViewBragBook.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = bragAdapter
        }
    }

    private fun setupListeners() {
        binding.backArrowImageView.setOnClickListener {
            findNavController().navigateUp()
        }

        binding.fabAddBrag.setOnClickListener {
            findNavController().navigate(R.id.action_bragBookFragment_to_createBragFragment)
        }
    }

//    private fun setupBottomNavigation() {
//        binding.bottomNavigation.selectedItemId = R.id.nav_home
//        binding.bottomNavigation.setOnItemSelectedListener { item ->
//            when (item.itemId) {
//                R.id.nav_home -> {
//                    findNavController().navigate(R.id.action_bragBookFragment_to_homePageFragment)
//                    true
//                }
//                R.id.nav_menu -> {
//                    findNavController().navigate(R.id.action_bragBookFragment_to_settingsFragment)
//                    true
//                }
//                else -> false
//            }
//        }
//    }

    private fun fetchBragPosts() {
        lifecycleScope.launch {
            try {
                val response = ApiClient.apiService.getBragPosts()
                val body = response.body()

                if (response.isSuccessful && body != null && body.data != null) {
                    bragAdapter.updateData(body.data)
                } else {
                    Toast.makeText(requireContext(), "Failed to load posts", Toast.LENGTH_SHORT).show()
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