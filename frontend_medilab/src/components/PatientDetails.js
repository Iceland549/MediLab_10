import React, { useState, useEffect } from "react";
import NotesList from "./NotesList";
import RiskLevel from "./RiskLevel";
import { fetchNotes } from "../services/api";

export default function PatientDetails({ patient }) {
  const [notes, setNotes] = useState([]);

  // À chaque nouveau patient, on recharge la liste des notes
  useEffect(() => {
    if (patient?.id) {
      fetchNotes(patient.id).then(setNotes);
    }
  }, [patient?.id]);

  if (!patient) return null;

  return (
    <div>
      <h2>Détails du patient</h2>
      <p>Nom : {patient.lastName}</p>
      <p>Prénom : {patient.firstName}</p>
      <p>Date de naissance : {patient.dateOfBirth}</p>
      <p>Genre : {patient.gender}</p>
      {patient.address && <p>Adresse : {patient.address}</p>}
      {patient.phoneNumber && <p>Téléphone : {patient.phoneNumber}</p>}

      {/* On passe directement notes et setNotes au composant enfant */}
      <NotesList
        patientId={patient.id}
        notes={notes}
        setNotes={setNotes}
      />

      <RiskLevel 
        patientId={patient.id}
        notes={notes} />
    </div>
  );
}
