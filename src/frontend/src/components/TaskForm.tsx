import React, { useState } from "react";
import { CreateTaskRequest } from "../services/api";

interface TaskFormProps {
  onSubmit: (task: CreateTaskRequest) => void;
}

export function TaskForm({ onSubmit }: TaskFormProps) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [priority, setPriority] = useState("Medium");
  const [dueDate, setDueDate] = useState("");
  const [assignedTo, setAssignedTo] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      title,
      description: description || undefined,
      priority,
      dueDate: dueDate || undefined,
      assignedTo: assignedTo || undefined,
    });
    setTitle("");
    setDescription("");
    setPriority("Medium");
    setDueDate("");
    setAssignedTo("");
  };

  return (
    <form onSubmit={handleSubmit} style={{ marginBottom: 24, padding: 16, border: "1px solid #ddd", borderRadius: 8 }}>
      <h3>Add Task</h3>
      <div style={{ display: "flex", flexDirection: "column", gap: 8 }}>
        <input
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
          style={{ padding: 8, fontSize: 14 }}
        />
        <textarea
          placeholder="Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          style={{ padding: 8, fontSize: 14 }}
        />
        <select value={priority} onChange={(e) => setPriority(e.target.value)} style={{ padding: 8 }}>
          <option>Low</option>
          <option>Medium</option>
          <option>High</option>
          <option>Critical</option>
        </select>
        <input
          type="date"
          value={dueDate}
          onChange={(e) => setDueDate(e.target.value)}
          style={{ padding: 8 }}
        />
        <input
          placeholder="Assigned to"
          value={assignedTo}
          onChange={(e) => setAssignedTo(e.target.value)}
          style={{ padding: 8 }}
        />
        <button type="submit" style={{ padding: "8px 16px", background: "#0070f3", color: "white", border: "none", borderRadius: 4, cursor: "pointer" }}>
          Add Task
        </button>
      </div>
    </form>
  );
}
