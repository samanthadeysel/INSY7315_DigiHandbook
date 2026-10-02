package com.example.employeedigitalhandbook.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.data.Quiz

class QuizAdapter(
    private var quizList: List<Quiz>,
    private val onQuizClick: (Quiz) -> Unit
) : RecyclerView.Adapter<QuizAdapter.QuizViewHolder>() {

    inner class QuizViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val txtQuizTitle: TextView = itemView.findViewById(R.id.txtQuizTitle)
        private val txtTopics: TextView = itemView.findViewById(R.id.txtTopics)
        private val txtTime: TextView = itemView.findViewById(R.id.txtTime)
        private val txtPoints: TextView = itemView.findViewById(R.id.txtPoints)

        fun bind(quiz: Quiz) {
            txtQuizTitle.text = quiz.title
            txtTopics.text = quiz.topic
            txtTime.text = "est. ${quiz.estimatedMinutes} min"

            // Formats double (e.g. 10.0 -> "10 pts" or 10.5 -> "10.5 pts")
            val formattedPoints = if (quiz.points % 1.0 == 0.0) {
                "${quiz.points.toInt()} pts"
            } else {
                "${quiz.points} pts"
            }
            txtPoints.text = formattedPoints

            itemView.setOnClickListener { onQuizClick(quiz) }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): QuizViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_quiz_card, parent, false)
        return QuizViewHolder(view)
    }

    override fun onBindViewHolder(holder: QuizViewHolder, position: Int) {
        holder.bind(quizList[position])
    }

    override fun getItemCount(): Int = quizList.size

    fun updateQuizzes(newList: List<Quiz>) {
        this.quizList = newList
        notifyDataSetChanged()
    }
}