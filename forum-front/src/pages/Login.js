import { useState } from "react";
import api from "../services/api";
import styles from './CSSsup/Login.module.css';
export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");

  const login = async () => {
    try {
      const res = await api.post("/auth/login", {
        email,
        password
      });

      localStorage.setItem("token", res.data.token);
      setMessage("Connecté avec succès 🍇");
    } catch (err) {
      setMessage("Erreur de connexion ❌");
    }
  };

 return (
    <div className={styles.formContainer}>
      <input
        className={styles.inputField}
        placeholder="Email"
        value={email}
        onChange={e => setEmail(e.target.value)}
      />
      <input
        className={styles.inputField}
        type="password"
        placeholder="Mot de passe"
        value={password}
        onChange={e => setPassword(e.target.value)}
      />
      <button
        className={styles.submitButton}
        onClick={login}
      >
        Se connecter
      </button>

      {message && <p className={styles.message}>{message}</p>}
    </div>
  );
}
