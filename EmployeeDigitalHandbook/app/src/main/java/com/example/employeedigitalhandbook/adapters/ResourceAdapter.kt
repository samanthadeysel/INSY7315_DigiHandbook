package com.example.employeedigitalhandbook.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.data.Resource

class ResourceAdapter(
    private var resourcesList: List<Resource>,
    private val onItemClick: (Resource) -> Unit
) : RecyclerView.Adapter<ResourceAdapter.ResourceViewHolder>() {

    class ResourceViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val txtPolicyTitle: TextView = itemView.findViewById(R.id.txtPolicyTitle)
        val txtPolicySummary: TextView = itemView.findViewById(R.id.txtPolicySummary)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ResourceViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_policy_card, parent, false)
        return ResourceViewHolder(view)
    }

    override fun onBindViewHolder(holder: ResourceViewHolder, position: Int) {
        val resource = resourcesList[position]

        holder.txtPolicyTitle.text = resource.title
        holder.txtPolicySummary.text = resource.description ?: ""

        holder.itemView.setOnClickListener {
            onItemClick(resource)
        }
    }

    override fun getItemCount(): Int = resourcesList.size

    fun updateData(newResources: List<Resource>) {
        resourcesList = newResources
        notifyDataSetChanged()
    }
}