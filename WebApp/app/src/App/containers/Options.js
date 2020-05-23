import React from 'react';
import axios from 'axios';
import {
    Link
  } from "react-router-dom";

  class Options extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            password: '',
            accounttype: 'proprietarios'
        };

    }


    
    render() {
        return (
            <div className="container login-container">
                <div className="row">
                    <div className="col"></div>
                    <div className="col-md-10">
                        <div className="card login-card">
                            <div className="card-block">

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default Options;