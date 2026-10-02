package com.example.employeedigitalhandbook.features

import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.ViewModelProvider
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.adapters.PolicyAdapter
import com.example.employeedigitalhandbook.data.Doctor
import com.example.employeedigitalhandbook.repositories.DoctorResult
import com.example.employeedigitalhandbook.viewmodel.DoctorViewModel
import com.google.android.material.bottomnavigation.BottomNavigationView
import com.google.android.material.button.MaterialButton

class DoctorFragment : Fragment() {

    private lateinit var doctorViewModel: DoctorViewModel
    private lateinit var policyAdapter: PolicyAdapter

    private lateinit var imgDoctorProfile: ImageView
    private lateinit var txtDoctorName: TextView
    private lateinit var txtDoctorSpecialty: TextView
    private lateinit var txtDoctorSuite: TextView
    private lateinit var txtDoctorPhone: TextView
    private lateinit var txtDoctorEmail: TextView
    private lateinit var btnCallDoctor: MaterialButton
    private lateinit var btnEmailDoctor: MaterialButton
    private lateinit var btnBack: ImageButton

    private var currentPhone: String = ""
    private var currentEmail: String = ""

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.fragment_doctor, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        imgDoctorProfile = view.findViewById(R.id.imgDoctorProfile)
        txtDoctorName = view.findViewById(R.id.txtDoctorName)
        txtDoctorSpecialty = view.findViewById(R.id.txtDoctorSpecialty)
        txtDoctorSuite = view.findViewById(R.id.txtDoctorSuite)
        txtDoctorPhone = view.findViewById(R.id.txtDoctorPhone)
        txtDoctorEmail = view.findViewById(R.id.txtDoctorEmail)
        btnCallDoctor = view.findViewById(R.id.btnCallDoctor)
        btnEmailDoctor = view.findViewById(R.id.btnEmailDoctor)
        btnBack = view.findViewById(R.id.btnBack)

        val recyclerViewPolicies = view.findViewById<RecyclerView>(R.id.doctorPoliciesRecyclerView)
        recyclerViewPolicies.layoutManager = LinearLayoutManager(requireContext())
        policyAdapter = PolicyAdapter(emptyList()) { selectedPolicy ->
            Toast.makeText(requireContext(), selectedPolicy.title, Toast.LENGTH_SHORT).show()
        }
        recyclerViewPolicies.adapter = policyAdapter

        btnBack.setOnClickListener {
            findNavController().navigateUp()
        }

        btnCallDoctor.setOnClickListener {
            if (currentPhone.isNotBlank()) {
                val intent = Intent(Intent.ACTION_DIAL, Uri.parse("tel:$currentPhone"))
                startActivity(intent)
            } else {
                Toast.makeText(requireContext(), "Phone number not available", Toast.LENGTH_SHORT).show()
            }
        }

        btnEmailDoctor.setOnClickListener {
            if (currentEmail.isNotBlank()) {
                val intent = Intent(Intent.ACTION_SENDTO).apply {
                    data = Uri.parse("mailto:$currentEmail")
                }
                startActivity(intent)
            } else {
                Toast.makeText(requireContext(), "Email address not available", Toast.LENGTH_SHORT).show()
            }
        }

        doctorViewModel = ViewModelProvider(this)[DoctorViewModel::class.java]

        doctorViewModel.doctorDetailsState.observe(viewLifecycleOwner) { result ->
            when (result) {
                is DoctorResult.Success -> {
                    bindDoctorDetails(result.doctor)
                }
                is DoctorResult.Error -> {
                    Toast.makeText(requireContext(), result.message, Toast.LENGTH_LONG).show()
                }
                else -> {

                }
            }
        }

        val doctorId = arguments?.getInt("doctorId") ?: 1
        doctorViewModel.loadDoctorDetails(doctorId)

        val bottomNavigation = view.findViewById<BottomNavigationView>(R.id.bottomNavigation)
        bottomNavigation.selectedItemId = R.id.nav_home
        bottomNavigation.setOnItemSelectedListener { item ->
            when (item.itemId) {
                R.id.nav_home -> {
                    findNavController().navigate(R.id.homePageFragment)
                    true
                }
                R.id.nav_menu -> {
                    findNavController().navigate(R.id.settingsFragment)
                    true
                }
                else -> false
            }
        }
    }

    private fun bindDoctorDetails(doctor: Doctor) {
        txtDoctorName.text = doctor.fullNameWithTitle
        txtDoctorSpecialty.text = "Opthalmologist"
        txtDoctorSuite.text = "Suite ${doctor.suiteNumber}"
        txtDoctorPhone.text = doctor.phone
        txtDoctorEmail.text = doctor.email

        currentPhone = doctor.phone
        currentEmail = doctor.email
    }
}