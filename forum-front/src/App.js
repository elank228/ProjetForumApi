import Login from "./pages/Login";
import Orb from "./modulesReact/Orb";
import TextType from './modulesReact/TextType';

function App() {
  return (
    <div
      style={{
        width: "100vw",
        height: "100vh",
        backgroundColor: "black",
        position: "relative",
        overflow: "hidden",
      }}
    >
      {/* Orb на фоне */}
      <Orb
        hoverIntensity={0.5}
        rotateOnHover={true}
        hue={4}
        forceHoverState={false}
      />

      {/* Login поверх Orb */}
      <div
        style={{
          position: "absolute",
          top: 0,
          left: 0,
          width: "100%",
          height: "100%",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          zIndex: 10,
          color: "white",
        }}
      >
        <div
          style={{
            background: "rgba(0,0,0,0.6)",
            padding: "40px",
            borderRadius: "12px",
            backdropFilter: "blur(10px)",
          }}
        >
          <h1> <TextType
            text={["Forum API"]}
            typingSpeed={75}
            pauseDuration={1500}
            showCursor={true}
            cursorCharacter="|"
          /></h1>
          <Login />
        </div>
      </div>
    </div>
  );
}
export default App;

