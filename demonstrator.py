# Code from python3 components in demonstrator.gh

# %% Setter component

"""
Script to 'set' global variables within grasshopper doc. Updates the 'get' components upon changes.
    Inputs:
        keys: a list of keys for the global variables
        values: a list of values associated to the keys
    Output:
        global_vars: 'key:value' pairs
        types: 'type(key):type(value)
"""

import scriptcontext as rs
import Grasshopper
import json

# Set the component name to something identifiable, to be controlled from other components.
component_name = 'Global Var Setter'
if ghenv.Component.Name != component_name:
    ghenv.Component.Name = component_name

# Add key:value pairs to the current grasshopper script context.
rs.sticky[keys] = values

global_vars = f'{keys}:' + f'{rs.sticky[keys]}'
types = f'{type(keys)}:' + f'{type(rs.sticky[keys])}'

# Expire all 'getter' components within the current document to ensure they are 'getting' the latest global variables.
ghdoc = ghenv.Component.OnPingDocument()

for obj in ghdoc.Objects:
    if obj.Name == 'Global Var Getter':
        obj.ExpireSolution(True)

# %% Getter component

"""
Script to 'get' the value of global variables within grasshopper doc.
    Inputs:
        key: the key of the global variable to get 
    Output:
        value: the value associated with the key
        value_type: the type of the value
"""

import scriptcontext as rs
import Grasshopper
import json

# Set the component name to something identifiable, to be controlled from the setter component.
component_name = 'Global Var Getter'
if ghenv.Component.Name != component_name:
    ghenv.Component.Name = component_name

# Obtain the value associated with the provided key.
try:
    value = rs.sticky[key]
except Exception as e:
    print('A value associated with the provided key could not be found.')

try:
    value_type = type(value)
except Exception as e:
    print('The type of the value could not be determined.')


