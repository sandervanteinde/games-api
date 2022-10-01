import React from "react";
import { Player } from "../models/player";

export const PlayerContext = React.createContext<null | Player>(null);
