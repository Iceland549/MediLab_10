import React, { useEffect, useState } from "react";
import { fetchRisk } from "../services/api";

export default function RiskLevel({ patientId, notes }) {
  const [risk, setRisk] = useState(null);

  useEffect(() => {
    if (patientId) {
      fetchRisk(patientId)
        .then(setRisk)
        .catch(() => setRisk(null));
    }
  }, [patientId, notes]);

  if (!patientId) return null;
  if (!risk) return <div>Risque : ...</div>;

  return (
    <div>
      <h3>Évaluation du risque</h3>
      <p>Niveau : {risk.riskLevel}</p>
      <p>Âge : {risk.age} ans</p>
      <p>Genre : {risk.gender}</p>
      {risk.notes && (
        <ul>
          {risk.notes.map((n, idx) => (
            <li key={idx}>{n}</li>
          ))}
        </ul>
      )}
    </div>
  );
}