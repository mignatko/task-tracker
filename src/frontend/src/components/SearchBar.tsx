import React, { useState } from "react";
import { searchTasks, TaskItem } from "../services/api";

interface SearchBarProps {
  onResults: (tasks: TaskItem[]) => void;
  onClear: () => void;
}

export function SearchBar({ onResults, onClear }: SearchBarProps) {
  const [query, setQuery] = useState("");

  const handleSearch = async () => {
    if (query.trim()) {
      const results = await searchTasks(query);
      onResults(results);
    }
  };

  const handleClear = () => {
    setQuery("");
    onClear();
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === "Enter") handleSearch();
  };

  return (
    <div style={{ display: "flex", gap: 8, marginBottom: 16 }}>
      <input
        placeholder="Search tasks..."
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        onKeyDown={handleKeyDown}
        style={{ flex: 1, padding: 8, fontSize: 14 }}
      />
      <button onClick={handleSearch} style={{ padding: "8px 16px", background: "#0070f3", color: "white", border: "none", borderRadius: 4, cursor: "pointer" }}>
        Search
      </button>
      {query && (
        <button onClick={handleClear} style={{ padding: "8px 12px", background: "#6b7280", color: "white", border: "none", borderRadius: 4, cursor: "pointer" }}>
          Clear
        </button>
      )}
    </div>
  );
}
