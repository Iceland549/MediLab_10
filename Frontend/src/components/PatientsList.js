import React, { useEffect, useState } from "react";
import { fetchPatients } from "../services/api";

export default function PatientsList({ onSelect }) {
  const [patients, setPatients] = useState([]);

  useEffect(() => {
    fetchPatients().then(setPatients);
  }, []);

  return (
    <div>
      <h2>Liste des patients</h2>
      <ul>
        {patients.map((patient) => (
          <li key={patient.id}>
            <button onClick={() => onSelect(patient)}>
              {patient.lastName} {patient.firstName}
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}
