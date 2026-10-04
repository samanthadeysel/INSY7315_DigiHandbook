package com.example.employeedigitalhandbook.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.data.CommunityEvent

class EventAdapter(
    private var events: List<CommunityEvent>,
    private val onItemClick: (CommunityEvent) -> Unit
) : RecyclerView.Adapter<EventAdapter.EventViewHolder>() {

    inner class EventViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val title: TextView = itemView.findViewById(R.id.txtEventTitle)
        val date: TextView = itemView.findViewById(R.id.txtEventDate)
        val time: TextView = itemView.findViewById(R.id.txtEventTime)
        val location: TextView = itemView.findViewById(R.id.txtEventLocation)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): EventViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_community_event_card, parent, false)
        return EventViewHolder(view)
    }

    override fun onBindViewHolder(holder: EventViewHolder, position: Int) {
        val event = events[position]
        holder.title.text = event.title
        holder.date.text = event.date
        holder.time.text = event.time
        holder.location.text = event.location

        holder.itemView.setOnClickListener { onItemClick(event) }
    }

    override fun getItemCount(): Int = events.size

    fun updateData(newEvents: List<CommunityEvent>) {
        events = newEvents
        notifyDataSetChanged()
    }
}