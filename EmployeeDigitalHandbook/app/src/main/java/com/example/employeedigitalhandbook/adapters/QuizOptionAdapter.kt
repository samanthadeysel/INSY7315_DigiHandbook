package com.example.employeedigitalhandbook.adapters

import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.data.QuizOption
import com.google.android.material.card.MaterialCardView

class QuizOptionAdapter(
    private var optionList: List<QuizOption>,
    private val onOptionClick: (QuizOption) -> Unit
) : RecyclerView.Adapter<QuizOptionAdapter.OptionViewHolder>() {

    private var selectedPosition = RecyclerView.NO_POSITION

    inner class OptionViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val cardOption: MaterialCardView = itemView.findViewById(R.id.cardOption)
        val imgRadioCheck: ImageView = itemView.findViewById(R.id.imgRadioCheck)
        val txtOptionText: TextView = itemView.findViewById(R.id.txtOptionText)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): OptionViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_quiz_option, parent, false)
        return OptionViewHolder(view)
    }

    override fun onBindViewHolder(holder: OptionViewHolder, position: Int) {
        val option = optionList[position]
        holder.txtOptionText.text = option.optionText

        val isSelected = (position == selectedPosition)

        if (isSelected) {
            holder.cardOption.strokeColor = Color.parseColor("#2690CF")
            holder.imgRadioCheck.setColorFilter(Color.parseColor("#2690CF"))
        } else {
            holder.cardOption.strokeColor = Color.parseColor("#CBD5E1")
            holder.imgRadioCheck.setColorFilter(Color.parseColor("#94A3B8"))
        }

        // Tap listener executes only on user interaction
        holder.itemView.setOnClickListener {
            val currentPosition = holder.bindingAdapterPosition
            if (currentPosition == RecyclerView.NO_POSITION) return@setOnClickListener

            val previousSelected = selectedPosition
            selectedPosition = currentPosition

            if (previousSelected != RecyclerView.NO_POSITION) {
                notifyItemChanged(previousSelected)
            }
            notifyItemChanged(selectedPosition)

            onOptionClick(option)
        }
    }

    override fun getItemCount(): Int = optionList.size

    fun updateOptions(newList: List<QuizOption>) {
        this.optionList = newList
        this.selectedPosition = RecyclerView.NO_POSITION
        notifyDataSetChanged()
    }

    fun getSelectedOption(): QuizOption? {
        return if (selectedPosition in optionList.indices) optionList[selectedPosition] else null
    }
}