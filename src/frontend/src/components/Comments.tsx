import React, { useEffect, useState } from "react";
import { Comment, getComments, addComment, deleteComment } from "../services/api";

interface CommentsProps {
  taskId: number;
}

export function Comments({ taskId }: CommentsProps) {
  const [comments, setComments] = useState<Comment[]>([]);
  const [author, setAuthor] = useState(localStorage.getItem("commentAuthor") || "");
  const [content, setContent] = useState("");
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    if (isOpen) {
      loadComments();
    }
  }, [isOpen]);

  const loadComments = async () => {
    const data = await getComments(taskId);
    setComments(data);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    localStorage.setItem("commentAuthor", author);
    await addComment(taskId, author, content);
    setContent("");
    loadComments();
  };

  const handleDelete = async (commentId: number) => {
    await deleteComment(taskId, commentId);
    loadComments();
  };

  if (!isOpen) {
    return (
      <button
        onClick={() => setIsOpen(true)}
        style={{ fontSize: 12, padding: "2px 8px", background: "transparent", border: "1px solid #ccc", borderRadius: 4, cursor: "pointer" }}
      >
        Comments
      </button>
    );
  }

  return (
    <div style={{ marginTop: 8, padding: 12, background: "#f9fafb", borderRadius: 6, fontSize: 13 }}>
      <div style={{ display: "flex", justifyContent: "space-between", marginBottom: 8 }}>
        <strong>Comments</strong>
        <button onClick={() => setIsOpen(false)} style={{ background: "none", border: "none", cursor: "pointer" }}>x</button>
      </div>

      {comments.map((c) => (
        <div key={c.id} style={{ marginBottom: 8, padding: 8, background: "white", borderRadius: 4 }}>
          <div style={{ display: "flex", justifyContent: "space-between" }}>
            <strong>{c.author}</strong>
            <span style={{ display: "flex", gap: 8, alignItems: "center" }}>
              <span style={{ color: "#999", fontSize: 11 }}>{new Date(c.createdAt).toLocaleString()}</span>
              <button onClick={() => handleDelete(c.id)} style={{ background: "none", border: "none", color: "red", cursor: "pointer", fontSize: 11 }}>delete</button>
            </span>
          </div>
          <div dangerouslySetInnerHTML={{ __html: c.content }} />
        </div>
      ))}

      <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: 4 }}>
        <input
          placeholder="Your name"
          value={author}
          onChange={(e) => setAuthor(e.target.value)}
          style={{ padding: 6, fontSize: 12 }}
        />
        <textarea
          placeholder="Add a comment..."
          value={content}
          onChange={(e) => setContent(e.target.value)}
          style={{ padding: 6, fontSize: 12 }}
        />
        <button type="submit" style={{ padding: "4px 12px", fontSize: 12, background: "#0070f3", color: "white", border: "none", borderRadius: 4, cursor: "pointer" }}>
          Post
        </button>
      </form>
    </div>
  );
}
