
import React, { useState } from "react";
import Login from "./components/Login";
import PatientsList from "./components/PatientsList";
import PatientDetails from "./components/PatientDetails";
import { getToken, removeToken } from "./services/api";
import './App.css';


function App() {
  const [token, setToken] = useState(getToken());
  const [selectedPatient, setSelectedPatient] = useState(null);

  const handleLogout = () => {
    removeToken();
    setToken(null);
    setSelectedPatient(null);
  };

  if (!token) {
    return <Login onLogin={setToken} />;
  }

  return (
    <div>
      <header>
        <h1>MediLab</h1>
        <button onClick={handleLogout}>Déconnexion</button>
      </header>

      {!selectedPatient ? (
        <PatientsList onSelect={setSelectedPatient} />
      ) : (
        <>
          <button onClick={() => setSelectedPatient(null)}>
            Retour à la liste
          </button>
          <PatientDetails patient={selectedPatient} />
        </>
      )}
    </div>
  );
}

export default App;


