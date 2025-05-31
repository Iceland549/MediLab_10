import axios from "axios";

const API_BASE_URL = process.env.REACT_APP_API_URL || "http://localhost:5000/api";

// Gestion du token JWT dans le localStorage
export function setToken(token) {
  localStorage.setItem("token", token);
}

export function getToken() {
  return localStorage.getItem("token");
}

export function removeToken() {
  localStorage.removeItem("token");
}

// Création d'une instance axios avec intercepteur JWT
const api = axios.create({
  baseURL: API_BASE_URL,
});

api.interceptors.request.use(
  (config) => {
    const token = getToken();
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

export default api;

// Fonctions d’appel à l’API**
// **Authentification**
export async function login(userName, password) {
  const response = await api.post("/auth/login", { userName, password });
  return response.data.token;
}

// **Patients**
export async function fetchPatients() {
  const response = await api.get("/patients");
  return response.data;
}

export async function fetchPatientById(id) {
  const response = await api.get(`/patients/${id}`);
  return response.data;
}

// **Notes**
export async function fetchNotes(patientId) {
  const response = await api.get(`/notes?patientId=${patientId}`);
  return response.data;
}

export async function addNote(patientId, content) {
  const response = await api.post("/notes", { patientId, content });
  return response.data;
}

export async function updateNote(noteId, content) {
  const response = await api.put(`/notes/${noteId}`, { content });
  return response.data;
}

export async function deleteNote(noteId) {
  const response = await api.delete(`/notes/${noteId}`);
  return response.data;
}

// **Risk**
export async function fetchRisk(patientId) {
  const response = await api.get(`/risk/${patientId}`);
  return response.data;
}