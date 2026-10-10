# PMB Eye Hospital - Employee Digital Handbook
A centralized digital platform designed for PMB Eye Hospital staff to access hospital policies, complete training quizzes, view peer recognition via the Brag Book, and review institutional resources from both mobile and desktop environments.

### Live Endpoints
* **Web Management Portal**: https://pmbeye-handbook.cloud.run
* **Backend API**: https://handbook-api-770247469632.europe-west3.run.app

### Video Demonstration
Watch the system walkthrough, mobile app demonstration, and architecture breakdown:
**Link: ->** https://youtu.be/S20jrU2Z2rQ?si=phSekfi24oBhqF-s

### Links to platforms
**Admin Portal Link: ->** https://pmbeye-handbook.cloud.run/
**API Link: ->** https://handbook-api-770247469632.europe-west3.run.app
**App Distribution Link: ->** https://appdistribution.firebase.dev/i/3fcb8e311dd0df55

### Ctrl Alt Elite Team Contributions
| Team Member | Primary Roles & Focus Areas | Key Deliverables & Responsibilities |
| :--- | :--- | :--- |
| **Nia Thandolwethu Cele** | Frontend Engineering (Mobile & Web), DevOps & Cloud Architecture | • **Android Mobile App:** Developed the full native Android client in Kotlin (UI screens, navigation, state handling, and API integration for policies, quizzes, and the Brag Book).<br><br>• **MVC Management Portal:** Built the entire ASP.NET Core MVC web portal frontend and admin dashboard views.<br><br>• **DevOps & Cloud:** Authored multi-stage Dockerfiles (`Dockerfile`, `Dockerfile.portal`), configured Google Cloud Build pipelines, and deployed both services to Google Cloud Run.<br><br>• **System Integration:** Managed multi-region networking, Cloud SQL PostgreSQL socket proxies, Google Cloud Storage integration, and resolved cross-service production routing issues. |
| **Samantha Abigail Deysel** | Backend API Architecture & Database Engineering | • **Backend Web API:** Developed the centralized ASP.NET Core Web API from the ground up (controllers, services, business logic, and DTOs).<br><br>• **Database & ORM:** Designed Entity Framework Core schemas, managed database migrations, and implemented data access layers for PostgreSQL / Cloud SQL.<br><br>• **Authentication & Endpoints:** Built and tested core API endpoints for authentication (`/api/Auth`), policy management, quiz evaluation, and peer recognition feeds. |
| **Sizwe Thandaza Majola** | Team Member | • Project contributor. |
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

| Component | Platform | Region / Host | Details |
| :--- | :--- | :--- | :--- |
| **Web Portal** | Google Cloud Run | `europe-west1` | ASP.NET Core MVC management portal served via custom URL (`pmbeye-handbook.cloud.run`). |
| **Backend API** | Google Cloud Run | `europe-west3` | Serverless, autoscaling REST API built on `.NET 10`. |
| **Database** | Google Cloud SQL | `africa-south1` | Managed PostgreSQL instance (`digital-handbook-db`) connected via Cloud SQL Auth Proxy Unix sockets. |
| **Object Storage** | Google Cloud Storage | Multi-Region | Secure cloud bucket (`digihandbook-bucket`) storing policy documents and media. |
| **Container Registry** | Artifact Registry / GCR | GCP Central | Versioned Docker images built and tagged for zero-downtime service revisions. |

---

## 3. DevOps & CI/CD Pipeline

The project integrates automated building, testing, container packaging, and deployment:

* **Source Control**: GitHub repository with feature branches and protected integration branches.
* **Continuous Integration (GitHub Actions)**:
  * Triggers on pull requests and pushes to `main`.
  * Restores dependencies, linting, and compiles the MVC portal, Android APK and Web API.

* **Continuous Deployment & Containerization (Docker + Cloud Build)**:
  * Multi-stage Docker files compiles and packages the API and Dashboard into container images.
  * Google Cloud Build automatically handles image builds directly from source code.
  * Google Cloud Run deploys new container revisions for both the API and Web Portal with zero downtime, smoothly routing live traffic to the latest build.
* **Configuration & Security**: 
  * Sensitive data (database connection strings, JWT keys, and storage bucket names) are kept out of source code.
  * Production secrets and inter-service endpoints are managed securely via Cloud Run environment variables and service account credentials.
  * Secure PostgreSQL communication is maintained using Cloud SQL Auth Proxy Unix domain sockets

---

## 4. Repository Structure
```plaintext
├── .github/workflows/          # contains all the github actions
├── Digital_Handbook_Portal/
│   ├── Digital_Handbook_Portal/ # ASP.NET Core MVC Web Application - Admin Portal
│   └── HandbookApi/             # ASP.NET Core Web API (Controllers, Models, Services) - API
├── EmployeeDigitalHandbook/     # Android Application (Kotlin, Gradle) - Staff-facing mobile app
├── Dockerfile                  # Container build definition for API
├── Dockerfile.portal            # Container build definition for Web Portal
└── README.md                   # Project documentation
