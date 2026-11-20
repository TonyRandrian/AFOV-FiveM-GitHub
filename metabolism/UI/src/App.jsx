import { useState, useEffect } from "react";
import "./App.css";
import Gauge from "./components/Gauge";
import BodyRepresentation  from "./components/BodyRepresentation";

function App() {
  const [isHudVisible, setHudVisible] = useState(false);
  const [isAllHudVisible, setAllHudVisible] = useState(true);
  const [gauges, setGauges] = useState(null);
  const [bodyParts, setBodyParts] = useState(null);

  useEffect(() => {
    const handler = (event) => {
      if (event.data.action === "setHudVisible") {
        setHudVisible(event.data.value);
      }
      else if (event.data.action === "updateState") {
        setGauges(event.data.value);
      }
      else if (event.data.action === "setAllHudVisible") {
        setAllHudVisible(event.data.value);
      }
      else if (event.data.action === "updateBodyParts") {
        setBodyParts(event.data.value);
      }
    };

    window.addEventListener("message", handler);
    return () => window.removeEventListener("message", handler);
  }, []);

  return (
    <div id="container">
      {
        gauges && bodyParts && isAllHudVisible && (
          <div id="permanent-gauge-container">
            <Gauge
              key="PV"
              label="PV"
              color="rgb(0, 85, 0)"
              value={bodyParts.Health.Health}
              maxValue={bodyParts.Health.MaxHealth}
            />
            {
              gauges.Permanents.map(gauge => (
                <Gauge
                  key={gauge.Name}
                  label={gauge.Name}
                  color="rgb(0, 85, 0)"
                  value={gauge.Value}
                  maxValue={gauge.Max}
                />
              ))
            }
          </div>
        )
      }

      {
        bodyParts && isHudVisible && isAllHudVisible && (
          <div id="body-part-container" /*style={{ display: "flex", flexDirection: "column", color: "red" }}*/>
            {
              <BodyRepresentation />
              // bodyParts.BodyParts.map(bodyPart => (
              //   <div>
              //     <div>{bodyPart.BodyPart}</div>
              //     <div>{bodyPart.Health}</div>
              //   </div>
              // ))
            }
          </div>
        )
      }

      {
        gauges && isHudVisible && isAllHudVisible && (
          <div id="hidden-gauge-container">
            {
              gauges.Hiddens.map(gauge => (

                gauge.Name !== "Temperature" ?
                  <Gauge
                    key={gauge.Name}
                    label={gauge.Name}
                    color="rgb(128, 126, 126)"
                    value={gauge.Value}
                    maxValue={gauge.Max} />
                  :
                  <div>
                    <span>{gauge.Name}</span> :
                    <span> {gauge.Value}</span>
                  </div>
              ))
            }
          </div>
        )
      }
    </div>
  );
}

export default App;
