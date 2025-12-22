import api from "../services/api";
import { useEffect, useState } from "react";

export default function Secure() {
  const [data, setData] = useState("");

  useEffect(() => {
    api.get("/auth/secure")
      .then(res => setData(res.data))
      .catch(() => setData("Accès refusé"));
  }, []);

  return <h2>{data}</h2>;
}
