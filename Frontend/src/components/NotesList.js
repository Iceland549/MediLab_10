import React, { useEffect, useState } from "react";
import { fetchNotes, addNote, updateNote, deleteNote } from "../services/api";

export default function NotesList({ patientId, notes, setNotes }) {
  const [newContent, setNewContent] = useState("");
  const [editingId, setEditingId] = useState(null);
  const [editingContent, setEditingContent] = useState("");

  // Recharge les notes au premier rendu ou à chaque changement de patient
  useEffect(() => {
    if (patientId) {
      fetchNotes(patientId).then(setNotes);
    }
  }, [patientId, setNotes]);

  // Ajout
  const handleAdd = async (e) => {
    e.preventDefault();
    if (!newContent.trim()) return;

    try {
      await addNote(patientId, newContent);
      // On refetch pour être sûr d'avoir les données à jour
      const updated = await fetchNotes(patientId);
      setNotes(updated);
    } catch (err) {
      console.error(err);
      alert("Erreur lors de l'ajout");
    }
    setNewContent("");
  };

  // Mise en edition
  const handleEdit = (note) => {
    setEditingId(note.id);
    setEditingContent(note.content);
  };

  // Validation de la modif
  const handleUpdate = async (noteId) => {
    try {
      await updateNote(noteId, editingContent);
      const updated = await fetchNotes(patientId);
      setNotes(updated);
      setEditingId(null);
      setEditingContent("");
    } catch (err) {
      console.error(err);
      alert("Erreur lors de la mise à jour");
    }
  };

  // Suppression
  const handleDelete = async (noteId) => {
    try {
      await deleteNote(noteId);
      const updated = await fetchNotes(patientId);
      setNotes(updated);
    } catch (err) {
      console.error(err);
      alert("Erreur lors de la suppression");
    }
  };

  return (
    <div>
      <h3>Notes médicales</h3>
      <ul>
        {notes.map((note) =>
          editingId === note.id ? (
            <li key={note.id}>
              <input
                type="text"
                value={editingContent}
                onChange={(e) => setEditingContent(e.target.value)}
              />
              <button onClick={() => handleUpdate(note.id)}>Valider</button>
              <button onClick={() => setEditingId(null)}>Annuler</button>
            </li>
          ) : (
            <li key={note.id}>
              {note.content}
              <button onClick={() => handleEdit(note)}>Modifier</button>
              <button onClick={() => handleDelete(note.id)}>Supprimer</button>
            </li>
          )
        )}
      </ul>

      <form onSubmit={handleAdd}>
        <input
          type="text"
          value={newContent}
          onChange={(e) => setNewContent(e.target.value)}
          placeholder="Nouvelle note"
        />
        <button type="submit">Ajouter</button>
      </form>
    </div>
  );
}
