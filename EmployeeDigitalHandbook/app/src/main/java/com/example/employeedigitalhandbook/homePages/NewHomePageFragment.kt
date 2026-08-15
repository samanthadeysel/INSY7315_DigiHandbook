package com.example.employeedigitalhandbook.homePages

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import com.example.employeedigitalhandbook.R
import com.google.android.material.card.MaterialCardView

class NewHomePageFragment : Fragment() {

    private lateinit var cardPolicies : MaterialCardView
    private lateinit var cardBragBook : MaterialCardView
    private lateinit var cardCommunity : MaterialCardView
    private lateinit var cardCPD : MaterialCardView


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {

        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_new_home_page, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        cardPolicies = view.findViewById(R.id.cardPolicies)
        cardBragBook = view.findViewById(R.id.cardBragBook)
        cardCommunity = view.findViewById(R.id.cardCommunity)
        cardCPD = view.findViewById(R.id.cardCPD)

        cardPolicies.setOnClickListener {
            findNavController().navigate(R.id.action_newHomePageFragment_to_policiesFrontFragment)
        }

        cardBragBook.setOnClickListener {
            findNavController().navigate(R.id.action_newHomePageFragment_to_bragBookFragment)
        }

        cardCommunity.setOnClickListener {
            findNavController().navigate(R.id.action_newHomePageFragment_to_communityFragment)
        }

        cardCPD.setOnClickListener {
            findNavController().navigate(R.id.action_newHomePageFragment_to_cpdFragment)
        }
    }
}