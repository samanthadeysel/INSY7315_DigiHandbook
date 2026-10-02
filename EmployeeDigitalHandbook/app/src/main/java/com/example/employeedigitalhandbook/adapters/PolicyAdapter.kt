package com.example.employeedigitalhandbook.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.data.Policy

class PolicyAdapter(
    private var policiesList: List<Policy>,
    private val onItemClick: (Policy) -> Unit
) : RecyclerView.Adapter<PolicyAdapter.PolicyViewHolder>() {

    class PolicyViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val txtPolicyTitle: TextView = itemView.findViewById(R.id.txtPolicyTitle)
        val txtPolicySummary: TextView = itemView.findViewById(R.id.txtPolicySummary)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): PolicyViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_policy_card, parent, false)
        return PolicyViewHolder(view)
    }

    override fun onBindViewHolder(holder: PolicyViewHolder, position: Int) {
        val policy = policiesList[position]
        holder.txtPolicyTitle.text = policy.title
        holder.txtPolicySummary.text = policy.summary ?: ""

        holder.itemView.setOnClickListener {
            onItemClick(policy)
        }
    }

    override fun getItemCount(): Int = policiesList.size

    fun updateData(newPolicies: List<Policy>) {
        policiesList = newPolicies
        notifyDataSetChanged()
    }
}