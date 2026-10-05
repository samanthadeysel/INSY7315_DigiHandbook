# PMB Eye Hospital - Employee Digital Handbook
A centralized digital platform designed for PMB Eye Hospital staff to access hospital policies, complete training quizzes, view peer recognition via the Brag Book, and review institutional resources from both mobile and desktop environments.

### Video Demonstration
Watch the system walkthrough, mobile app demonstration, and architecture breakdown:

> **Link: ->**

### Ctrl Alt Elite Team
* Nia Thandolwethu Cele
* Samantha Abigail Deysel
* Sizwe Thandaza Majola 
---

## 1. System Architecture

The solution uses a decoupled architecture which spans across mobile(Android App_, web portal(ASP.NET Core MVC), centralized API(ASP.NET Core Web API), and cloud storage layers (Cloud SQL and Google Cloud Storage)

* **Mobile App (Android/Kotlin)**: The staff-facing app used to read policies, complete quizzes, and post peer recognition on the go.
* **Web Portal (ASP.NET Core MVC)**: The admin dashboard for hospital managers to update handbook policies, manage staff records, and configure settings.
* **Backend API (ASP.NET Core Web API)**: The central engine that connects the apps to the database, handling logins, content delivery, quizzes, and community features.
* **Database (PostgreSQL/Cloud SQL)**: Secure cloud database storing structured data like user profiles, quiz results, brag book posts, and policy information.
* **File Storage (Google Cloud Storage)**: loud bucket (digihandbook-bucket) used for storing uploaded files like policy PDF documents and media.

---

## 2. Infrastructure & Hosting

All backend components are containerized and hosted on **Google Cloud Platform (GCP)** in the `europe-west3` region:

| Component | Platform | Details |
| :--- | :--- | :--- |
| **API Runtime** | Google Cloud Run | Serverless, autoscaling containerized deployment running `.NET 10`. |
| **Database** | Google Cloud SQL | Managed PostgreSQL instance connected via Cloud SQL Auth Proxy / Unix socket. |
| **Object Storage** | Google Cloud Storage | Secure bucket storage for policy documents and media assets. |
| **Container Registry** | Google Artifact Registry / GCR | Versioned Docker images built and tagged for Cloud Run revisions. |

---

## 3. DevOps & CI/CD Pipeline

The project integrates automated building, testing, container packaging, and deployment:

* **Source Control**: GitHub repository with feature branches and protected integration branches.
* **Continuous Integration (GitHub Actions)**:
  * Triggers on pull requests and pushes to `main`.
  * Restores dependencies and compiles both the MVC portal and Web API.

* **Continuous Deployment & Containerization (Docker + Cloud Build)**:
  * Multi-stage Dockerfile compiles and packages the API into a container image.
  * Google Cloud Build automatically handles image builds directly from source code.
  * Google Cloud Run deploys new container revisions with zero downtime, smoothly routing live traffic to the latest build.
* **Configuration & Security**: 
  * Sensitive data (database connection strings, JWT keys, and storage bucket names) are kept out of source code.
  * Production secrets are managed securely via Cloud Run environment variables and service account credentials.

---

## 4. Repository Structure
```plaintext
├── .github/workflows/          # contains all the github actions
├── Digital_Handbook_Portal/
│   ├── Digital_Handbook_Portal/ # ASP.NET Core MVC Web Application - Admin Portal
│   └── HandbookApi/             # ASP.NET Core Web API (Controllers, Models, Services) - API
├── EmployeeDigitalHandbook/     # Android Application (Kotlin, Gradle) - Staff-facing mobile app
├── Dockerfile                  # Container build definition
└── README.md
