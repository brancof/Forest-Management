import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";

  class Navbar extends React.Component {
    constructor(props) {
        super(props);

        };

        render() {
        
            return (
                <nav className="navbar navbar-light bg-light">
                    <a className="navbar-brand" href="#"> Terrenos </a>
                </nav>
            );
        }
  }

  export default Navbar