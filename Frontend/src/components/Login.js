import React, { useState } from "react";
import { login, setToken } from "../services/api";

export default function Login({ onLogin }) {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const token = await login(userName, password);
      setToken(token);
      onLogin(token);
    } catch {
      setError("Identifiant ou mot de passe incorrect");
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Connexion MediLab</h2>
      <div>
        <label>Nom d'utilisateur</label>
        <input
          type="text"
          value={userName}
          onChange={e => setUserName(e.target.value)}
          required
          autoFocus
        />
      </div>
      <div>
        <label>Mot de passe</label>
        <input
          type="password"
          value={password}
          onChange={e => setPassword(e.target.value)}
          required
        />
      </div>
      {error && <div style={{color: "red"}}>{error}</div>}
      <button type="submit">Se connecter</button>
    </form>
  );
}
