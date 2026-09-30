package com.example.employeedigitalhandbook.homePages

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import android.widget.Toast
import androidx.cardview.widget.CardView
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.adapters.DoctorAdapter
import com.example.employeedigitalhandbook.data.Doctor
import com.example.employeedigitalhandbook.repositories.DoctorResult
import com.example.employeedigitalhandbook.viewmodel.HomeViewModel
import com.google.android.material.bottomnavigation.BottomNavigationView
import java.util.Calendar

class HomePageFragment : Fragment() {

    private lateinit var HomeViewModel: HomeViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_home_page, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        HomeViewModel = ViewModelProvider(this)[HomeViewModel::class.java]

        val tvGreetingPrefix = view.findViewById<TextView>(R.id.tvGreetingPrefix)
        val tvUserName = view.findViewById<TextView>(R.id.tvUserName)

        tvGreetingPrefix.text = getGreetingPrefix()

        val loggedInUser = arguments?.getString("USER_NAME")
        if (!loggedInUser.isNullOrBlank()) {
            tvUserName.text = "$loggedInUser!"
        } else {
            tvUserName.text = "User!"
        }

        view.findViewById<CardView>(R.id.cardPolicies).setOnClickListener {
            findNavController().navigate(R.id.action_homePageFragment_to_policiesFrontFragment)
        }
        view.findViewById<CardView>(R.id.cardBragBook).setOnClickListener {
            findNavController().navigate(R.id.action_homePageFragment_to_bragBookFragment)
        }
        view.findViewById<CardView>(R.id.cardCommunity).setOnClickListener {
            findNavController().navigate(R.id.action_homePageFragment_to_communityFragment)
        }
        view.findViewById<CardView>(R.id.cardCPD).setOnClickListener {
            findNavController().navigate(R.id.action_homePageFragment_to_comingSoonFragment)
        }

        val doctorsRecyclerView = view.findViewById<RecyclerView>(R.id.doctorsRecyclerView)
        doctorsRecyclerView.layoutManager = LinearLayoutManager(requireContext(), LinearLayoutManager.HORIZONTAL, false)

        HomeViewModel.doctorsState.observe(viewLifecycleOwner) { result ->
            when (result) {
                is DoctorResult.Success -> {
                    doctorsRecyclerView.adapter = DoctorAdapter(result.doctors) { doctor ->
                        Toast.makeText(
                            requireContext(),
                            "${doctor.fullNameWithTitle} - Suite ${doctor.suiteNumber}",
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                }
                is DoctorResult.Error -> {
                    Toast.makeText(requireContext(), result.message, Toast.LENGTH_LONG).show()
                }
            }
        }

        HomeViewModel.loadDoctors()

        val bottomNavigation = view.findViewById<BottomNavigationView>(R.id.bottomNavigation)
        bottomNavigation.selectedItemId = R.id.nav_home
        bottomNavigation.setOnItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_home -> true
                R.id.nav_settings -> {
                    findNavController().navigate(R.id.action_homePageFragment_to_settingsFragment)
                    true
                }
                else -> false
            }
        }
    }

    private fun getGreetingPrefix(): String {
        return when (Calendar.getInstance().get(Calendar.HOUR_OF_DAY)) {
            in 0..11 -> "Good morning, "
            in 12..16 -> "Good afternoon, "
            else -> "Good evening, "
        }
    }
}